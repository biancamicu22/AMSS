import { Observable } from "rxjs";
import { TaskModel } from "src/app/shared/models/taskModel";
import { TaskOperationStrategy } from "../TaskOperationStrategy";
import { TaskOperationType } from "../TaskOperationTypes";

export class TaskStatusStrategy extends TaskOperationStrategy{
    isApplicable(): boolean {
        return this.opType == TaskOperationType.TaskStatus;
    }
    applyStrategy(status: string): Observable<any> {
        if(status == 'ToDo') return this.ToDoStatus(this.task);
        if(status == 'InProgress') return this.InProgressStatus(this.task);
        return this.DoneStatus(this.task);
    }

    InProgressStatus(task: TaskModel): Observable<any> {
        return this.http.put(this.baseApiUrl + "task/inprogress/" + task.id, task)
    }

    ToDoStatus(task: TaskModel): Observable<any> {
        return this.http.put(this.baseApiUrl + "task/todo/" + task.id, task)
    }

    DoneStatus(task: TaskModel): Observable<any> {
        return this.http.put(this.baseApiUrl + "task/done/" + task.id, task)
    }
}