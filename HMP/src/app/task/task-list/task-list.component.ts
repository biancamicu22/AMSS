
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { TaskModel } from 'src/app/shared/models/taskModel';
import { TaskService } from 'src/app/shared/services/task.service';
import { CustomLoaderService } from 'src/app/shared/services/customLoader.service';
import { SprintService } from 'src/app/shared/services/sprint.service';
import { TaskOperationType } from 'src/app/shared/strategies/tasks/TaskOperationTypes';

@Component({
  selector: 'app-task-list',
  templateUrl: 'task-list.component.html'
})

export class TaskListComponent implements OnInit {
  constructor(public taskService: TaskService, private customService: CustomLoaderService,
    private changeDetector: ChangeDetectorRef, private router: Router, private sprintService: SprintService) { }

  @ViewChild('taskNameFilter') taskNameFilter: HTMLInputElement;
  detectChange() {
    this.changeDetector.detectChanges();
    console.log('change_' + this.taskNameFilter.value);
  }

  ngOnInit() {
    this.customService.start();
    this.taskService.getAllTasks(this.sprintService.selectedSprint.id).subscribe(r => {
      this.customService.stop();
    }, this.customService.errorFromResp);
  }

  setSelectedTask(task: TaskModel) {
    this.taskService.showCreateForm(task);
  }

  deleteTask(task: TaskModel) {
    this.customService.start();
    this.taskService.performTaskOperation(TaskOperationType.TaskDelete, task).subscribe(r => {
      this.customService.success('Task was deleted successfully!', 'Success');
      this.taskService.getAllTasks(this.sprintService.selectedSprint.id).subscribe(resp => {
        this.customService.stop();
      }, this.customService.errorFromResp)
    }, this.customService.errorFromResp)
  }

  seeTasks(task: TaskModel) {
    this.taskService.selectedTask = task;
    this.router.navigate(['/tasks/' + task.id]);
  }

  statusChanged(task: TaskModel, event) {
    task.status = event;
    this.customService.start();
    this.taskService.performTaskOperation(TaskOperationType.TaskStatus, task, event).subscribe(res => {
       this.customService.success('Task was updated successfully!', 'Success'); 
       this.customService.stop();
    });
  }
}