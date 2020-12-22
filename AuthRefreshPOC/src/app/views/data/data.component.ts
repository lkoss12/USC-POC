import { Component, OnInit } from '@angular/core';
import { DataService } from 'src/services/data.service';

@Component({
  selector: 'app-data',
  templateUrl: './data.component.html',
  styleUrls: ['./data.component.css']
})
export class DataComponent implements OnInit {

  values: string[];
  constructor(private dataService: DataService) { }

  ngOnInit(): void {
  }
  loadData() {
    const dataSub = this.dataService
      .LoadData()
      .subscribe((result: any[]) => {
        this.values = result.map(x => x.value);
        dataSub.unsubscribe();
      });

  }
}
