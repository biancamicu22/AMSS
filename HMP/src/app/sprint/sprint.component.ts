import { Component, OnInit } from '@angular/core';
import { SprintService } from 'src/app/shared/services/sprint.service';

@Component({
    selector: 'app-sprint',
    templateUrl: 'sprint.component.html'
})

export class SprintComponent implements OnInit {
    
    public createMode: boolean = false;

    constructor(public sprintService: SprintService) {
        this.createMode = this.sprintService.createFormVisible;
        this.sprintService.selectedSprintChanged.subscribe(data => {
            this.createMode = data.showCreate;
        });
     }

    ngOnInit() { }
}