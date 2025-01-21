import { inject, Injectable, isDevMode } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpService {


  private httpClient = inject(HttpClient);
  private origin = location.origin;

  BACKEND_URL =  isDevMode() ? this.origin + '/api/' : `https://crimean-billing.work.gd/api/`;

  public get<T>(method: string) {
    return this.httpClient.get<T>(`${this.BACKEND_URL}${method}`, { withCredentials: true });
  }

  public post<T>(method: string, body?: any, options?: { headers: HttpHeaders, withCredentials?: boolean } | {[p: string] : string }) {
    if (!options) {
      options = {}
    }
    options.withCredentials = true
    return this.httpClient.post<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public put<T>(method: string, body: any, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.put<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public patch<T>(method: string, body: any, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.patch<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public delete(method: string, options?: { withCredentials?: boolean, headers: HttpHeaders | {[p: string] : string} }) {
    if (!options) {
      options = {} as unknown as any
    }
    options!.withCredentials = true
    return this.httpClient.delete(`${this.BACKEND_URL}${method}`, options);
  }
}
