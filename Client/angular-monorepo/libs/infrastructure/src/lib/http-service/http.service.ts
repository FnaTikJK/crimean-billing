import { inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class HttpService {

  BACKEND_URL = 'http://www.crimean-billing.work.gd/api/'

  private httpClient = inject(HttpClient);
  private origin = location.origin;

  public get<T>(method: string) {
    return this.httpClient.get<T>(`${this.BACKEND_URL}${method}`);
  }

  public post<T>(method: string, body?: any, options?: { headers: HttpHeaders } | {[p: string] : string }) {
    return this.httpClient.post<T>(`${this.BACKEND_URL}${method}`, body, options);
  }

  public put(method: string, body: any, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.put(`${this.BACKEND_URL}${method}`, body, options);
  }

  public patch(method: string, body: any, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.patch(`${this.BACKEND_URL}${method}`, body, options);
  }

  public delete(method: string, options?: { headers: HttpHeaders | {[p: string] : string} }) {
    return this.httpClient.delete(`${this.BACKEND_URL}${method}`, options);
  }
}
