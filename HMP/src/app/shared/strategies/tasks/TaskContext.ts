import { HttpClient } from "@angular/common/http";
import { Observable } from "rxjs";
import { TaskModel } from "../../models/taskModel";
import { TaskDeleteStrategy } from "./strategies/TaskDeleteStrategy";
import { TaskEditStrategy } from "./strategies/TaskEditStrategy";
import { TaskStatusStrategy } from "./strategies/TaskStatusChangeStartegy";
import { TaskOperationStrategy } from "./TaskOperationStrategy";
import { TaskOperationType } from "./TaskOperationTypes";

export class TaskContext {
    taskStrategies: TaskOperationStrategy[] = [];
    status: string;
    constructor(opType: TaskOperationType, task: TaskModel,  http: HttpClient, baseApiUrl: string, status: string = ''){
        this.status = status;
        this.taskStrategies.push(new TaskEditStrategy(opType, task, http, baseApiUrl));
        this.taskStrategies.push(new TaskStatusStrategy(opType, task, http, baseApiUrl));
        this.taskStrategies.push(new TaskDeleteStrategy(opType, task, http, baseApiUrl));
    }

    performOperation(): Observable<any>{
        return this.taskStrategies.find(strategy => strategy.isApplicable()).applyStrategy(this.status);
    }
}