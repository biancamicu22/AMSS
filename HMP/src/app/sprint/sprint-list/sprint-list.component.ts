
import { ChangeDetectorRef, Component, OnInit, ViewChild } from '@angular/core';
import { Router } from '@angular/router';
import { SprintModel } from 'src/app/shared/models/sprintModel';
import { SprintService } from 'src/app/shared/services/sprint.service';
import { CustomLoaderService } from 'src/app/shared/services/customLoader.service';
import { TaskService } from 'src/app/shared/services/task.service';

@Component({
    selector: 'app-sprint-list',
    templateUrl: 'sprint-list.component.html'
})

export class SprintListComponent implements OnInit {
    constructor(public sprintService: SprintService, private customService: CustomLoaderService, 
        private changeDetector: ChangeDetectorRef, private router: Router, public taskService: TaskService) { }

    @ViewChild('sprintNameFilter') sprintNameFilter: HTMLInputElement;
    detectChange(){
        this.changeDetector.detectChanges();
        console.log('change_' + this.sprintNameFilter.value);
    }

    ngOnInit() {
        this.customService.start();
        this.sprintService.getAllSprints().subscribe(r => {
            this.customService.stop();
        }, this.customService.errorFromResp);
     }

     setSelectedSprint(sprint: SprintModel){
         this.sprintService.showCreateForm(sprint);
     }

     deleteSprint(sprint: SprintModel){
        this.sprintService.deleteSprint(sprint).subscribe(r => {
                this.customService.success('Sprint was deleted!', 'Success');
                  this.sprintService.getAllSprints().subscribe(resp => {
                      this.customService.stop();
                  }, this.customService.errorFromResp)
        }, this.customService.errorFromResp)
     }

     seeTasks(sprint: SprintModel){
        this.taskService.hideCreateForm();
        this.sprintService.selectedSprint = sprint;
        this.router.navigate(['/tasks']);
     }
}