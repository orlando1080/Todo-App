import {
  inject,
  Injectable,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { catchError, EMPTY, Observable, tap } from 'rxjs';

import { TasksClient, TaskItemDto } from '@api';

@Injectable({
  providedIn: 'root',
})
export class TaskService {
  private _tasks: WritableSignal<TaskItemDto[]> = signal<TaskItemDto[]>([]);
  public tasksSignal: Signal<TaskItemDto[]> = this._tasks.asReadonly(); // Publicly read-only for safety

  private tasksClient: TasksClient = inject(TasksClient);

  public loadTasks(): void {
    this.tasksClient
      .getAll()
      .pipe(
        // 'tap' is like a "Side Effect" - look but don't touch
        tap((data) => this._tasks.set(data)),
        catchError((err: any): Observable<never> => {
          console.error('Failed to load tasks', err);
          return EMPTY;
        }),
      )
      .subscribe(); // Trigger the request
  }

  public deleteTask(id: string): void {
    // Capture the old state (in case we need to roll back)
    const previousTasks: TaskItemDto[] = this._tasks();

    // Optimistic Update: Remove it from the UI immediately
    this._tasks.set(
      previousTasks.filter((item: TaskItemDto) => item.id !== id),
    );

    // Make the actual API call
    this.tasksClient.delete(id).subscribe({
      error: (err: any): void => {
        // If the ApI fails, roll back the UI
        console.error('Delete failed, rolling back!', err);
        this._tasks.set(previousTasks);
      },
    });
  }

  public createTask(title: string) {
    this.tasksClient
      .create(title)
      .pipe(
        tap((newTask) => {
          // Spread operator: Take old todos, add the new one
          this._tasks.update((current) => [...current, newTask]);
        }),
      )
      .subscribe();
  }

  public toggleTaskCompletion(id: string): void {
    this.tasksClient.toggleCompletion(id).subscribe({
      error: (err: any): void => {
        console.error('Failed to update task', err);
      },
      complete: () => {
        this._tasks.update((tasks) =>
          tasks.map((t) =>
            t.id === id ? { ...t, isCompleted: !t.isCompleted } : t,
          ),
        );
      },
    });
  }
}
