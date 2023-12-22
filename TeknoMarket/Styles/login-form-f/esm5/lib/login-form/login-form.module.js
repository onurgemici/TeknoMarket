/**
 * @fileoverview added by tsickle
 * Generated from: lib/login-form/login-form.module.ts
 * @suppress {checkTypes,constantProperty,extraRequire,missingOverride,missingReturn,unusedPrivateMembers,uselessCode} checked by tsc
 */
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { NgxCaptchaModule } from 'ngx-captcha';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { LoginFormComponent } from './login-form.component';
import { GoogleLoginModule } from '../google-login/google-login.module';
var LoginFormModule = /** @class */ (function () {
    function LoginFormModule() {
    }
    LoginFormModule.decorators = [
        { type: NgModule, args: [{
                    declarations: [LoginFormComponent],
                    imports: [
                        CommonModule,
                        NgxCaptchaModule,
                        GoogleLoginModule, FormsModule, ReactiveFormsModule
                    ],
                    exports: [LoginFormComponent], providers: []
                },] }
    ];
    return LoginFormModule;
}());
export { LoginFormModule };
/**
 * @return {?}
 */
export function LoginFormModuleExport() {
    return LoginFormModule;
}
//# sourceMappingURL=data:application/json;base64,eyJ2ZXJzaW9uIjozLCJmaWxlIjoibG9naW4tZm9ybS5tb2R1bGUuanMiLCJzb3VyY2VSb290Ijoibmc6Ly9sb2dpbi1mb3JtLWYvIiwic291cmNlcyI6WyJsaWIvbG9naW4tZm9ybS9sb2dpbi1mb3JtLm1vZHVsZS50cyJdLCJuYW1lcyI6W10sIm1hcHBpbmdzIjoiOzs7OztBQUFBLE9BQU8sRUFBQyxZQUFZLEVBQUMsTUFBTSxpQkFBaUIsQ0FBQztBQUM3QyxPQUFPLEVBQUMsUUFBUSxFQUFDLE1BQU0sZUFBZSxDQUFDO0FBRXZDLE9BQU8sRUFBQyxnQkFBZ0IsRUFBQyxNQUFNLGFBQWEsQ0FBQztBQUM3QyxPQUFPLEVBQUMsV0FBVyxFQUFFLG1CQUFtQixFQUFDLE1BQU0sZ0JBQWdCLENBQUM7QUFDaEUsT0FBTyxFQUFDLGtCQUFrQixFQUFDLE1BQU0sd0JBQXdCLENBQUM7QUFDMUQsT0FBTyxFQUFDLGlCQUFpQixFQUFDLE1BQU0scUNBQXFDLENBQUM7QUFFdEU7SUFBQTtJQVdBLENBQUM7O2dCQVhBLFFBQVEsU0FBQztvQkFDUixZQUFZLEVBQUUsQ0FBQyxrQkFBa0IsQ0FBQztvQkFDbEMsT0FBTyxFQUFFO3dCQUNQLFlBQVk7d0JBQ1osZ0JBQWdCO3dCQUNoQixpQkFBaUIsRUFBRSxXQUFXLEVBQUUsbUJBQW1CO3FCQUVwRDtvQkFDRCxPQUFPLEVBQUUsQ0FBQyxrQkFBa0IsQ0FBQyxFQUFFLFNBQVMsRUFBRSxFQUFFO2lCQUM3Qzs7SUFFRCxzQkFBQztDQUFBLEFBWEQsSUFXQztTQURZLGVBQWU7Ozs7QUFHNUIsTUFBTSxVQUFVLHFCQUFxQjtJQUNuQyxPQUFPLGVBQWUsQ0FBQztBQUN6QixDQUFDIiwic291cmNlc0NvbnRlbnQiOlsiaW1wb3J0IHtDb21tb25Nb2R1bGV9IGZyb20gJ0Bhbmd1bGFyL2NvbW1vbic7XHJcbmltcG9ydCB7TmdNb2R1bGV9IGZyb20gJ0Bhbmd1bGFyL2NvcmUnO1xyXG5cclxuaW1wb3J0IHtOZ3hDYXB0Y2hhTW9kdWxlfSBmcm9tICduZ3gtY2FwdGNoYSc7XHJcbmltcG9ydCB7Rm9ybXNNb2R1bGUsIFJlYWN0aXZlRm9ybXNNb2R1bGV9IGZyb20gJ0Bhbmd1bGFyL2Zvcm1zJztcclxuaW1wb3J0IHtMb2dpbkZvcm1Db21wb25lbnR9IGZyb20gJy4vbG9naW4tZm9ybS5jb21wb25lbnQnO1xyXG5pbXBvcnQge0dvb2dsZUxvZ2luTW9kdWxlfSBmcm9tICcuLi9nb29nbGUtbG9naW4vZ29vZ2xlLWxvZ2luLm1vZHVsZSc7XHJcblxyXG5ATmdNb2R1bGUoe1xyXG4gIGRlY2xhcmF0aW9uczogW0xvZ2luRm9ybUNvbXBvbmVudF0sXHJcbiAgaW1wb3J0czogW1xyXG4gICAgQ29tbW9uTW9kdWxlLFxyXG4gICAgTmd4Q2FwdGNoYU1vZHVsZSxcclxuICAgIEdvb2dsZUxvZ2luTW9kdWxlLCBGb3Jtc01vZHVsZSwgUmVhY3RpdmVGb3Jtc01vZHVsZVxyXG5cclxuICBdLFxyXG4gIGV4cG9ydHM6IFtMb2dpbkZvcm1Db21wb25lbnRdLCBwcm92aWRlcnM6IFtdXHJcbn0pXHJcbmV4cG9ydCBjbGFzcyBMb2dpbkZvcm1Nb2R1bGUge1xyXG59XHJcblxyXG5leHBvcnQgZnVuY3Rpb24gTG9naW5Gb3JtTW9kdWxlRXhwb3J0KCkge1xyXG4gIHJldHVybiBMb2dpbkZvcm1Nb2R1bGU7XHJcbn1cclxuIl19