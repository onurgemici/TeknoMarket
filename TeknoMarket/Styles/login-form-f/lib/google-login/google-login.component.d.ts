import { EventEmitter, OnInit } from '@angular/core';
import { AuthFacade } from 'af-services';
export declare class GoogleLoginComponent implements OnInit {
    private authFacade;
    isModal: boolean;
    saveAndRedirectToCurrentPage: boolean;
    done: EventEmitter<any>;
    constructor(authFacade: AuthFacade);
    ngOnInit(): Promise<void>;
    showLoading(): void;
}
