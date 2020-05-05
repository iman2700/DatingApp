import { Component, OnInit } from '@angular/core';
import {HttpClient} from '@angular/common/http';
import { error } from '@angular/compiler/src/util';

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.css']
})
export class ValueComponent implements OnInit {
valuse:any;
  constructor(private http:HttpClient) { }

  ngOnInit() {
    this.getValue();
  }
  getValue()
  {
    this.http.get('http://localhost:5000/api/value').subscribe(
     response => { this.valuse = response; }
    , error => { console.log(error); }
    );
  }

}
