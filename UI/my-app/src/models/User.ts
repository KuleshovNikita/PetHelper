import { BaseEntity } from "./BaseEntity";

export interface User extends BaseEntity {
    email: string;
    firstName: string;
    lastName: string;
    age?: string;
    token: string;
}

export interface UserRegisterModel {
    firstName: string;
    lastName: string;
    age?: string;
    login: string;
    password: string;
    repeatPassword: string;
}

export interface UserLoginModel {
    login: string;
    password: string;
}