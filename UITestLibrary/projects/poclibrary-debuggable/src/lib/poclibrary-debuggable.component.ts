import { Component, Input, OnInit } from '@angular/core';

@Component({
  selector: 'lib-poclibrary-debuggable',
  templateUrl: './poclibrary-debuggable.component.html',
  styles: [
  ]
})
export class PoclibraryDebuggableComponent implements OnInit {
  @Input() label: string[];
  @Input() btnLabel: any[];

  val: string;
  constructor() { }

  ngOnInit(): void {
  }
  clickBtn() {
    console.log('clicked in debug version!');
  }

}
