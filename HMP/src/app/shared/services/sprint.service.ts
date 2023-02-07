import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { SprintModel } from '../models/sprintModel';
import { Observable, Subject } from 'rxjs';
import { AppSettings } from 'src/app/app.settings';
import { map } from 'rxjs/operators';

export class SprintChangedModel {
    sprint: SprintModel;
    showCreate: boolean;
}

@Injectable()
export class SprintService {

    public sprintList: SprintModel[] = [];
    public selectedSprint: SprintModel;
    public createFormVisible: boolean = false;

    public selectedSprintChanged: Subject<SprintChangedModel>;
    constructor(private appSettings: AppSettings, private http: HttpClient){
        this.selectedSprintChanged = new Subject<SprintChangedModel>();
    }
    
    public showCreateForm(selectedSprint: SprintModel = null){
        this.selectedSprint = selectedSprint;
        this.createFormVisible = true;
        this.selectedSprintChanged.next({
            showCreate: true,
            sprint: selectedSprint
        });
    }

    public hideCreateForm(){
        this.selectedSprint = null;
        this.createFormVisible = false;
        this.selectedSprintChanged.next({
            showCreate: false,
            sprint: null
        });
    }

    public deleteSprint(sprint: SprintModel): Observable<any>{
        return this.http.delete(this.appSettings.baseApiUrl + "sprint/" + sprint.id);
    }

    public getAllSprints(): Observable<SprintModel[]>{
        return this.http.get<SprintModel[]>(this.appSettings.baseApiUrl + "sprint/all").pipe(map(list => {
            this.sprintList = list;
            return list;
        }));
    }

    public updateSprint(sprint: SprintModel): Observable<SprintModel>{
        return this.http.put<SprintModel>(this.appSettings.baseApiUrl + "sprint", sprint).pipe(map(x => {
            this.selectedSprint = x;
            return x;
        }));
    }

    public createSprint(sprint: SprintModel): Observable<SprintModel>{
        return this.http.post<SprintModel>(this.appSettings.baseApiUrl + "sprint", sprint).pipe(map(x => {
            this.selectedSprint = x;
            return x;
        }));
    }
}