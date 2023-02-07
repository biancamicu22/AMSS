import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { ModalDirective } from 'angular-bootstrap-md';

@Component({
    selector: 'app-custom-modal',
    templateUrl: 'custom-modal.component.html'
})

export class CustomModalComponent implements OnInit {
    
    
    @Input() public Title: string = '';
    @Input() public Body: string = '';
    @Input() public ShowClose: boolean = true;
    @Input() public ActionButtonText: string = '';
    @Input() public ActionButtonCallback: Function = null;
    @Input() public ShowModalFunc: Function = null;
    @Input() public HideModalFunc: Function = null;

    constructor() { }

    ngOnInit() {
        this.ShowModalFunc = () => {
            this.showModal();
        }
        this.HideModalFunc = () => {
            this.hideModal();
        }
     }

    @ViewChild('customModal') public showModalOnClick: ModalDirective;

    public showModal():void {
    
        this.showModalOnClick.show();
    
    }
    public hideModal():void {
    
        this.showModalOnClick.hide();
    
    }

    public triggerCallback(){
        if(this.ActionButtonCallback != null){
            this.ActionButtonCallback();
        }
    }
}