import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { TaskService } from 'src/app/shared/services/task.service';
import { SprintService } from '../shared/services/sprint.service';

@Component({
    selector: 'app-task',
    templateUrl: 'task.component.html'
})

export class TaskComponent implements OnInit {
    
    public createMode: boolean = false;

    public sprintName: string;
    constructor(public taskService: TaskService, public sprintService: SprintService, public router: Router) {
        if(this.sprintService.selectedSprint == null){
            this.router.navigate(['/sprints']);
        }
        this.taskService.selectedTaskChanged.subscribe(data => {
            this.createMode = data.showCreate;
        });
     }

    ngOnInit() {
        this.sprintName = this.sprintService.selectedSprint.name;
        if(this.sprintService.selectedSprint == null){
            this.router.navigate(['/sprints']);
        }
        this.createMode = this.taskService.createFormVisible;
    }
}