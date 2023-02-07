import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { TaskModel } from "../../models/taskModel";
import { TaskOperationType } from "./TaskOperationTypes";

export abstract class TaskOperationStrategy{

    opType: TaskOperationType;
    task: TaskModel;
    http: HttpClient;
    baseApiUrl: string;

    constructor(opType: TaskOperationType, task: TaskModel, http: HttpClient, baseApiUrl: string){
        this.opType = opType;
        this.task = task;
        this.http = http;
        this.baseApiUrl = baseApiUrl;
    }

    abstract isApplicable(): boolean;
    abstract applyStrategy(status: string): Observable<any>;
    
}