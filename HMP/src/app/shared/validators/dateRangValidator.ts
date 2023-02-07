import {FormGroup, ValidationErrors, ValidatorFn} from '@angular/forms';

export function creatDateRangeValidator(): ValidatorFn {
    return (form: FormGroup): ValidationErrors | null => {

        const start:Date = form.get("startDate").value;

        const end:Date = form.get("endDate").value;

        if (start && end) {
            const isRangeValid = (new Date(end).getTime() - new Date(start).getTime() > 0);

            return isRangeValid ? null : {dateRange:true};
        }

        return null;
    }
}