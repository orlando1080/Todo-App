import {
  Component,
  inject,
  OnInit,
  signal,
  Signal,
  WritableSignal,
} from '@angular/core';
import { FormsModule } from '@angular/forms';

import { TodoItemDto } from '@api';

import { TodoService } from '../../data-access/todo.service';

@Component({
  selector: 'app-todo-list',
  imports: [FormsModule],
  templateUrl: './todo-list.html',
  styleUrl: './todo-list.scss',
})
export class TodoList implements OnInit {
  public newTodoTitle: WritableSignal<string> = signal<string>('');

  protected _todoService: TodoService = inject(TodoService);

  // Expose the Signal to the template.
  // Notice we don't need to "subscribe" here!

  public todos: Signal<TodoItemDto[]> = this._todoService.todos;

  public ngOnInit(): void {
    this._todoService.getAll();
    console.log(this.todos());
  }

  public add(): void {
    const title: string = this.newTodoTitle().trim();

    if (title) {
      this._todoService.add(title);
      this.newTodoTitle.set(''); // Clear the input
    }
  }
}
