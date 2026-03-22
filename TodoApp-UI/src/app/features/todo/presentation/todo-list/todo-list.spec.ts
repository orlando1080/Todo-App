import { ComponentFixture, TestBed } from '@angular/core/testing';
import { DebugElement, WritableSignal } from '@angular/core';
import { By } from '@angular/platform-browser';

import { TodoItemDto } from '@api';

import { TodoService } from '../../data-access/todo.service';
import { createTodoServiceStub } from '../../data-access/todo.service.stub';

import { TodoList } from './todo-list';

describe('TodoList', () => {
  let component: TodoList;
  let fixture: ComponentFixture<TodoList>;
  let dependencies: {
    mockTodoService: TodoService;
  };
  let todoStubSignal: WritableSignal<TodoItemDto[]>;
  let todoServiceStub;

  function getText(): DebugElement {
    return fixture.debugElement.query(By.css('span'));
  }

  function getButton(): DebugElement {
    return fixture.debugElement.query(By.css('button'));
  }

  beforeEach(async () => {
    todoServiceStub = createTodoServiceStub();
    todoStubSignal = todoServiceStub.todoItems;

    dependencies = {
      mockTodoService: todoServiceStub.stub,
    };

    await TestBed.configureTestingModule({
      imports: [TodoList],
      providers: [
        {
          provide: TodoService,
          useValue: dependencies.mockTodoService,
        },
      ],
    }).compileComponents();

    fixture = TestBed.createComponent(TodoList);
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
      expect(dependencies.mockTodoService.getAll).toHaveBeenCalled();
    });

    it('should have 4 todos', () => {
      expect(fixture.debugElement.queryAll(By.css('li')).length).toEqual(4);
    });
  });

  describe('on initialisation with empty list', () => {
    beforeEach(() => {
      todoStubSignal.set([]);
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
      fixture.componentRef.instance.newTodoTitle.set(title);
      fixture.detectChanges();
      getButton().nativeElement.click();
    });

    it('should add task to the state service', () => {
      expect(dependencies.mockTodoService.add).toHaveBeenCalledWith(title);
    });

    it('should reset the input to empty', () => {
      expect(fixture.componentInstance.newTodoTitle()).toBe('');
    });
  });
});
