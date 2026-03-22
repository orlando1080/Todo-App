import { signal, WritableSignal } from '@angular/core';

import SpyObj = jasmine.SpyObj;

import { TodoService } from './todo.service';

import { TodoItemDto } from '@api';

export const createTodoServiceStub = () => {
  const todoItems: WritableSignal<TodoItemDto[]> = signal([]);

  // Define the spy object with the methods you want to mock
  const stub: SpyObj<TodoService> = jasmine.createSpyObj(
    'TodoService',
    ['getAll', 'delete', 'add'],

    // Define properties (like Signals) in the third argument
    { todos: todoItems.asReadonly() },
  );

  stub.getAll.and.callFake(() => {
    todoItems.set([
      { id: '1', title: 'Test Task', isCompleted: false },
      { id: '2', title: 'Test Task 2', isCompleted: false },
    ]);
  });
  return { stub, todoItems };
};
