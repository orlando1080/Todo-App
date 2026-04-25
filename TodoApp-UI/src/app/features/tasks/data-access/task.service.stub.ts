import { signal, WritableSignal } from '@angular/core';

import SpyObj = jasmine.SpyObj;

import { TaskService } from './task.service';

import { TaskItemDto } from '@api';

export const createTaskServiceStub = () => {
  const taskItems: WritableSignal<TaskItemDto[]> = signal([]);

  // Define the spy object with the methods you want to mock
  const stub: SpyObj<TaskService> = jasmine.createSpyObj(
    'TaskService',
    ['getAll', 'delete', 'add'],

    // Define properties (like Signals) in the third argument
    { tasks: taskItems.asReadonly() },
  );

  stub.loadTasks.and.callFake(() => {
    taskItems.set([
      { id: '1', title: 'Test Task', isCompleted: false },
      { id: '2', title: 'Test Task 2', isCompleted: false },
    ]);
  });
  return { stub, taskItems: taskItems };
};
