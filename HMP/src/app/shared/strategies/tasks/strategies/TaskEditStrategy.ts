import { Observable } from "rxjs";
import { map } from "rxjs/operators";
import { TaskModel } from "src/app/shared/models/taskModel";
import { TaskOperationStrategy } from "../TaskOperationStrategy";
import { TaskOperationType } from "../TaskOperationTypes";

export class TaskEditStrategy extends TaskOperationStrategy{
    isApplicable(): boolean {
        return this.opType == TaskOperationType.TaskEdit;
    }
    applyStrategy(): Observable<TaskModel> {
        return this.http.put<TaskModel>(this.baseApiUrl + "task", this.task);
    }
}