import { ApplicationConfig, ErrorHandler, Provider, provideZoneChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';
import { appRoutes } from './app.routes';
import { provideAnimationsAsync } from '@angular/platform-browser/animations/async';
import { provideHttpClient } from '@angular/common/http';
import { CustomErrorHandlerService } from '@angular-monorepo/infrastructure';
import { provideNativeDateAdapter } from '@angular/material/core';

const providers: Provider[] = [
  { provide: ErrorHandler, useClass: CustomErrorHandlerService }
]
export const appConfig: ApplicationConfig = {
  providers: [
    provideZoneChangeDetection({ eventCoalescing: true }),
    provideRouter(appRoutes), provideAnimationsAsync(),
    provideHttpClient(),
    provideNativeDateAdapter(),
    provideAnimationsAsync(),
    ...providers
  ],
};


