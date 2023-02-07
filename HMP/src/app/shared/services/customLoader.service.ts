import { Injectable } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';
import { ToastrService } from 'ngx-toastr';

@Injectable()
export class CustomLoaderService {
    constructor(private loaderService: NgxUiLoaderService, private toastr: ToastrService) {
    }
    
     public start = () => {
         this.loaderService.start();
     }

     public stop = () => {
         this.loaderService.stop();
     }

     public success = (message: string, title: string) => {
         this.toastr.success('', message);
     }

     public error =  (message: string, title: string) => {
         this.toastr.error(message, title,  {
            enableHtml: true,
        });
     }

     public errorFromResp =  (error: any) => {
            this.stop();

            let errorMsg = "";
            if(error.error.validationResults != null && error.error.validationResults != undefined){
                error.error.validationResults.forEach(element => {
                    errorMsg += `<div class="row"><div class="col">${element.fieldError}</div></div>`;
                });
            }
            else{
                errorMsg = error.error;
            }

            this.error(errorMsg, "Eroare");
     }

     public defaultError =  (_) => {
        this.stop();
        this.error('Ceva nu a functionat, va rugam sa reincercati!', 'Oops...');
     }
}