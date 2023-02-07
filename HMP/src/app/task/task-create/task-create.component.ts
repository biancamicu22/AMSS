import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { TaskModel } from 'src/app/shared/models/taskModel';
import { TaskService } from 'src/app/shared/services/task.service';
import { CustomLoaderService } from 'src/app/shared/services/customLoader.service';
import { UserService } from 'src/app/shared/services/user.service';
import { SprintModel } from 'src/app/shared/models/sprintModel';
import { UserModel } from 'src/app/shared/models/userModel';
import { SprintService } from 'src/app/shared/services/sprint.service';
import { TaskOperationType } from 'src/app/shared/strategies/tasks/TaskOperationTypes';

@Component({
    selector: 'app-task-create',
    templateUrl: 'task-create.component.html'
})

export class TaskCreateComponent implements OnInit {
    public taskForm: FormGroup;
    public otherErrorsDiv = null;
    public otherErrorsDivPassword = null;
    public otherErrorsDivTwofa = null;


    public sprintsList: SprintModel[];
    public usersList: UserModel[];

    constructor(public taskService: TaskService, private formBuilder: FormBuilder, private router: Router, public changeDetector: ChangeDetectorRef,
        private customService: CustomLoaderService, private userService: UserService, private sprintService: SprintService) { 
            this.sprintService.getAllSprints().subscribe(sprints => {
                this.sprintsList = sprints;
                this.userService.getAllUsers().subscribe(users => {
                    this.usersList = users;
                    this.taskService.selectedTaskChanged.subscribe(_ => {
                        this.reinitForms();
                    });
                    this.changeDetector.detectChanges();
                    let cSprint = sprints.find(b => b.id == this.taskService.selectedTask.sprint_id);
                    let cUser = users.find(b => b.id == this.taskService.selectedTask.user_id);
                    (document.getElementById('sprintSelect') as any).value = this.selectedSprint = cSprint.id;
                    (document.getElementById('userSelect') as any).value = this.selectedUser = cUser.id;  
                });
            });
        }

    ngOnInit() {
        this.reinitForms();
    }
    get f() { return this.taskForm.controls; }
    checkInput(input: any) {
        return input.invalid && (input.dirty || input.touched);
    }

    reinitForms(clear: boolean = false) {
        if(clear) this.taskService.selectedTask = null;
        this.taskForm = this.formBuilder.group({
            taskName: [this.taskService.selectedTask?.name, [Validators.required]],
            description: [this.taskService.selectedTask?.description, [Validators.required]],
            sprintSelect: ['', []],
            userSelect: ['', []],
        });
    }

    public atLeastOneEdit: boolean = false;
    onSubmit() {
        this.taskForm.markAllAsTouched();
        if (this.taskForm.invalid && this.taskService.selectedTask == null) {
            return;
        }
        if (this.taskService.selectedTask != null) {
            this.customService.start();
            this.taskService.performTaskOperation(TaskOperationType.TaskEdit, <TaskModel>{
                id: this.taskService.selectedTask.id,
                name: this.f.taskName.value,
                description: this.f.description.value,
                sprint_id: this.selectedSprint,
                user_id: this.selectedUser
            }).subscribe(r => {
                this.otherErrorsDiv = "";
                this.reinitForms();
                this.customService.success('Task was updated successfully!', 'Success');
                this.taskService.hideCreateForm();
            }, this.customService.errorFromResp);
        }
        else {
            this.customService.start();
            this.taskService.createTask(<TaskModel>{
                name: this.f.taskName.value,
                description: this.f.description.value,
                sprint_id: this.selectedSprint,
                user_id: this.selectedUser
            }).subscribe(response => {
                this.otherErrorsDiv = "";
                this.reinitForms();
                this.customService.success('Task was created successfully!', 'Success');
                this.taskService.hideCreateForm();
            }, this.customService.errorFromResp);
        }
    }

    selectedSprint: string = '-1';
    sprintChanged(event) {
        this.selectedSprint = event;
    }

    selectedUser: string = '-1';
    userChanged(event) {
        this.selectedUser = event
    }
    
}