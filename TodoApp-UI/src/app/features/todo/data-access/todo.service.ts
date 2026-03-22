import {
  inject,
  Injectable,
  Signal,
  signal,
  WritableSignal,
} from '@angular/core';
import { catchError, EMPTY, Observable, tap } from 'rxjs';

import { TodoClient, TodoItemDto } from '@api';

@Injectable({
  providedIn: 'root',
})
export class TodoService {
  private _todos: WritableSignal<TodoItemDto[]> = signal<TodoItemDto[]>([]);
  public todos: Signal<TodoItemDto[]> = this._todos.asReadonly(); // Publicly read-only for safety

  private todoClient: TodoClient = inject(TodoClient);

  public getAll(): void {
    this.todoClient
      .getAll()
      .pipe(
        // 'tap' is like a "Side Effect" - look but don't touch
        tap((data) => this._todos.set(data)),
        catchError((err: any): Observable<never> => {
          console.error('Failed to load tasks', err);
          return EMPTY;
        }),
      )
      .subscribe(); // Trigger the request
  }

  public delete(id: string): void {
    // Capture the old state (in case we need to roll back)
    const previousTodos: TodoItemDto[] = this._todos();

    // Optimistic Update: Remove it from the UI immediately
    this._todos.set(
      previousTodos.filter((item: TodoItemDto) => item.id !== id),
    );

    // Make the actual API call
    this.todoClient.delete(id).subscribe({
      error: (err: any): void => {
        // If the ApI fails, roll back the UI
        console.error('Delete failed, rolling back!', err);
        this._todos.set(previousTodos);
      },
    });
  }

  public add(title: string) {
    this.todoClient
      .create(title)
      .pipe(
        tap((newTodo) => {
          // Spread operator: Take old todos, add the new one
          this._todos.update((current) => [...current, newTodo]);
        }),
      )
      .subscribe();
  }
}
