import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DebugElement, WritableSignal } from '@angular/core';
import { By } from '@angular/platform-browser';

import { TaskItemDto } from '@api';

import { TaskService } from '../../data-access/task.service';
import { createTaskServiceStub } from '../../data-access/task.service.stub';

import { TaskList } from './task-list';

describe('TaskList', () => {
  let component: TaskList;
  let fixture: ComponentFixture<TaskList>;
  let dependencies: {
    mockTaskService: TaskService;
  };
  let taskStubSignal: WritableSignal<TaskItemDto[]>;
  let taskServiceStub;

  function getText(): DebugElement {
    return fixture.debugElement.query(By.css('span'));
  }

  function getButton(): DebugElement {
    return fixture.debugElement.query(By.css('button'));
  }

  beforeEach(async () => {
    taskServiceStub = createTaskServiceStub();
    taskStubSignal = taskServiceStub.taskItems;

    dependencies = {
      mockTaskService: taskServiceStub.stub,
    };

    await TestBed.configureTestingModule({
      imports: [TaskList],
      providers: [
        {
          provide: TaskService,
          useValue: dependencies.mockTaskService,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TaskList);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  describe('on initialisation', () => {
    it('should create', () => {
      expect(component).toBeTruthy();
    });

    it('should display tasks from the state service', () => {
      expect(getText().nativeElement?.textContent).toBe('Test Task');
    });

    it('should call getAll', () => {
      expect(dependencies.mockTaskService.loadTasks).toHaveBeenCalled();
    });

    it('should have 4 tasks', () => {
      expect(fixture.debugElement.queryAll(By.css('li')).length).toEqual(4);
    });
  });

  describe('on initialisation with empty list', () => {
    beforeEach(() => {
      taskStubSignal.set([]);
      fixture.detectChanges();
    });

    it('it should display no tasks yet message', () => {
      expect(fixture.debugElement.queryAll(By.css('p')).length).toEqual(2);
    });

    it('lists should be empty', () => {
      expect(fixture.debugElement.queryAll(By.css('li')).length).toEqual(0);
    });
  });

  describe('when user adds task', async () => {
    let title: string;

    beforeEach(() => {
      title = 'new title';
      fixture.componentRef.instance.taskTitleInput.set(title);
      fixture.detectChanges();
      getButton().nativeElement.click();
    });

    it('should add task to the state service', () => {
      expect(dependencies.mockTaskService.createTask).toHaveBeenCalledWith(
        title,
      );
    });

    it('should reset the input to empty', () => {
      expect(fixture.componentInstance.taskTitleInput()).toBe('');
    });
  });
});
