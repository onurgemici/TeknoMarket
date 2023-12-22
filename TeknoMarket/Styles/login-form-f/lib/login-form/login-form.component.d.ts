import { OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { AuthFacade, NotificationService } from 'af-services';
import { Router } from '@angular/router';
export declare class LoginFormComponent implements OnInit {
    private fb;
    private authFacade;
    private notify;
    captchaKey: any;
    private afterLoginRedirectTo;
    private router;
    form: FormGroup;
    captchaSuccess: boolean;
    show: boolean;
    error: any;
    callingControl: any;
    captchaError: boolean;
    constructor(fb: FormBuilder, authFacade: AuthFacade, notify: NotificationService, captchaKey: any, afterLoginRedirectTo: any, router: Router);
    ngOnInit(): Promise<void>;
    submit(callingControl: any): Promise<void>;
    showHidePassword(): void;
    handleSuccessCaptcha($event: string): void;
    private doLogin;
    private onSuccessfulLogin;
}
