import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskModel } from 'src/app/shared/models/taskModel';
import { CustomLoaderService } from 'src/app/shared/services/customLoader.service';
import { TaskService } from 'src/app/shared/services/task.service';
import { UserService } from 'src/app/shared/services/user.service';
import { TaskOperationType } from 'src/app/shared/strategies/tasks/TaskOperationTypes';
import { TaskListComponent } from '../task-list/task-list.component';

@Component({
  selector: 'app-task-update',
  templateUrl: 'task-update.component.html',
  styleUrls: ['./style-task.component.scss']
})

export class TaskUpdateComponent implements OnInit {
  id: string
  task: TaskModel
  form: FormGroup;
  public username: string = '';
  displayErrorMessage: boolean = false;

  constructor(public taskService: TaskService, private formBuilder: FormBuilder, public router: Router,
    private route: ActivatedRoute, private customService: CustomLoaderService, public userService: UserService) {
      this.username = this.userService.currentUserValue.username;
  } 

  ngOnInit() {
    this.createForm();
    this.route.params.subscribe(res => this.id = res["id"]);
    this.taskService.GetTaskById(this.id).subscribe(res => {
      this.task = res;
    })
  }

  createForm() {
    this.form = this.formBuilder.group({
      comment: ['', Validators.required],
    })
  }

  get f() { return this.form.controls; }
  checkInput(input: any) {
    return input.invalid && (input.dirty || input.touched);
  }

  addComment() {
    this.form.markAllAsTouched();
    if (this.form.invalid) {
      return;
    }
    this.customService.start();
    this.taskService.AddComment(this.task, this.form.controls.comment.value).subscribe({ next: res => {
      this.customService.success('Comment was added successfully!', 'Success');
      this.taskService.GetTaskById(this.id).subscribe(res => {
        this.task = res;
        this.createForm();
        this.customService.stop();
      })
    }, error: error => {
      this.displayErrorMessage = true;
      this.customService.error('Comment could not be added, please try again!', 'Error');
      setTimeout(()=>{this.displayErrorMessage=false;}, 5000);
      this.customService.stop();
    }})
  }

  statusChanged(event) {
    this.customService.start();
    this.task.status = event;
    
    this.taskService.performTaskOperation(TaskOperationType.TaskStatus, this.task, event).subscribe(res => {
      this.customService.success('Task was updated successfully!', 'Success');
      this.customService.stop();
    });
  }

  deleteComment(commentId: string){
    this.customService.start();
    this.taskService.DeleteComment(commentId).subscribe(res => {
      this.customService.success('Comment was deleted successfully!', 'Success');
      this.taskService.GetTaskById(this.id).subscribe(res => {
        this.task = res;
        this.customService.stop();
      })
    })
  }
}