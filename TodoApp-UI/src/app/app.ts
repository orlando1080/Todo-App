import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TodoList } from './features/todo/presentation/todo-list/todo-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TodoList],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  title = 'ToDoApp';
}
