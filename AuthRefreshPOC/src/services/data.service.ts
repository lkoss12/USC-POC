import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';

@Injectable({
    providedIn: 'root'
})
export class DataService {

    constructor(private httpClient: HttpClient)
    {
        
    }
    
    public LoadData()
    {
        return this.httpClient
            .get(environment.authUrl + "/data");
    }
}