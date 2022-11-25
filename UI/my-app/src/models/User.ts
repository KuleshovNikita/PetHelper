import { BaseEntity } from "./BaseEntity";

export interface User extends BaseEntity {
    login: string;
    firstName: string;
    lastName: string;
    age?: string;
    token: string;
}

export interface UserUpdateModel extends BaseEntity {
    login?: string;
    firstName?: string;
    lastName?: string;
    age?: string;
    password?: string;
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