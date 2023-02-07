import { Injectable } from '@angular/core';
import { NgxUiLoaderService } from 'ngx-ui-loader';
@Injectable()
export class AppSettings {
    public baseUrl: string = "https://localhost:44330/";
    public baseApiUrl: string = "https://localhost:44330/api/v1/";
    public lsUserDataKey: string = "userData";
    public lsAccountAddressExpiration: number = 30 * 60 * 1000; //30 mins in milisecs
}