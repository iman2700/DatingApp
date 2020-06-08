import { Component, OnInit, Input, EventEmitter, Output } from '@angular/core';
import { AuthService } from '../_services/auth.service';
import { error } from '@angular/compiler/src/util';
import { AlertifyService } from '../_services/alertify.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {
  @Input() valuesFromHome: any;
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  constructor(private authService: AuthService,private alertyfy:AlertifyService) {

   }
  ngOnInit() {
  }
  register()
  {
    this.authService.register(this.model).subscribe(() => {
      this.alertyfy.sucsses("registration is sucssecfull");
    }, error =>{
      this.alertyfy.error(error);
              });
  }
  cancel()
  {
   this.cancelRegister.emit(false);
   this.alertyfy.message("cancel this method");
  }

}
