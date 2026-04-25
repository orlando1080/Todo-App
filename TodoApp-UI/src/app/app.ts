import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';

import { TaskList } from './features/tasks/presentation/task-list/task-list';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, TaskList],
  templateUrl: './app.html',
  styleUrl: './app.scss',
})
export class App {
  title = 'ToDoApp';
}
