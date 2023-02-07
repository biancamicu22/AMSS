import { Component, OnInit, Input } from '@angular/core';
import { UserService } from 'src/app/shared/services/user.service';
import { SprintService } from '../services/sprint.service';
import { TaskService } from '../services/task.service';

@Component({
    selector: 'app-infobar',
    templateUrl: 'infobar.component.html'
})

export class InfobarComponent implements OnInit {

    @Input()
    public Route: string = '';
    @Input()
    public BackButtonValue: string = '';
    @Input()
    public ShowNewSprint: boolean = false;
    public sprintText = 'New sprint';
    
    @Input()
    public ShowNewTask: boolean = false;
    public taskText = 'New task';

    constructor(public userService: UserService, public sprintService: SprintService, public taskService: TaskService) {
        this.setSprintText();
        this.sprintService.selectedSprintChanged.subscribe(data => {
            this.setSprintText();
        });
        this.taskService.selectedTaskChanged.subscribe(data => {
            this.setTaskText();
        });
    }

    private setTaskText() {
        if (this.taskService.createFormVisible) {
            this.taskText = "See all tasks";
        }
        else {
            this.taskText = "New task";
        }
    }
    
    private setSprintText() {
        if (this.sprintService.createFormVisible) {
            this.sprintText = "See all sprints";
        }
        else {
            this.sprintText = "New sprints";
        }
    }
    showNewSprint() {
        if (this.sprintService.createFormVisible) {
            this.sprintService.hideCreateForm();
        }
        else {
            this.sprintService.showCreateForm();
        }
    }

    showNewTask() {
        if (this.taskService.createFormVisible) {
            this.taskService.hideCreateForm();
        }
        else {
            this.taskService.showCreateForm();
        }
    }

    ngOnInit() { }
}