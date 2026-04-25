import {
  Component,
  inject,
  OnInit,
  signal,
  Signal,
  WritableSignal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TaskItemDto } from '@api';

import { TaskService } from '../../data-access/task.service';

@Component({
  selector: 'app-todo-list',
  imports: [FormsModule],
  templateUrl: './task-list.html',
  styleUrl: './task-list.scss',
})
export class TaskList implements OnInit {
  public taskTitleInput: WritableSignal<string> = signal<string>('');

  protected _taskService: TaskService = inject(TaskService);

  // Expose the Signal to the template.
  // Notice we don't need to "subscribe" here!

  public tasks: Signal<TaskItemDto[]> = this._taskService.tasksSignal;

  public ngOnInit(): void {
    this._taskService.loadTasks();
    console.log(this.tasks());
  }

  public onCreateTask(): void {
    const title: string = this.taskTitleInput().trim();

    if (title) {
      this._taskService.createTask(title);
      this.taskTitleInput.set(''); // Clear the input
    }
  }

  public OnDeleteTask(id: string): void {
    this._taskService.deleteTask(id);
  }

  public onToggleCompletion(id: string): void {
    this._taskService.toggleTaskCompletion(id);
  }
}
