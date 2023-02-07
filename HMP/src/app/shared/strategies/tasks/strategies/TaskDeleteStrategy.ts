import { Observable } from "rxjs";
import { TaskOperationStrategy } from "../TaskOperationStrategy";
import { TaskOperationType } from "../TaskOperationTypes";

export class TaskDeleteStrategy extends TaskOperationStrategy{
    isApplicable(): boolean {
        return this.opType == TaskOperationType.TaskDelete;
    }
    applyStrategy(): Observable<any> {
        return this.http.delete(this.baseApiUrl + "task/" + this.task.id);
    }
}