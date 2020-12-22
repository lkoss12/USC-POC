import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'lib-POCLibrary',
  templateUrl: './poclibrary.component.html',
  styles: [
  ]
})
export class POCLibraryComponent implements OnInit {
  @Input() label: string[];
  @Input() btnLabel: any[];

  val: string;
  constructor() { }

  ngOnInit(): void {
  }
  clickBtn() {
    console.log('clicked!');
  }
}
