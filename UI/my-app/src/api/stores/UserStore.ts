import { makeAutoObservable, runInAction } from "mobx";
import { agent } from "../agent";
import { User, UserLoginModel, UserRegisterModel, UserUpdateModel } from "../../models/User";
import { store } from "./Store";
import { toast } from "react-toastify";


export default class UserStore {
    user: User | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    get isLoggedIn() {
        return !!this.user;
    }

    login = async (creds: UserLoginModel) => {
        const response = await agent.Auth.login(creds);

        if(!response.isSuccessful) {
            console.log(response);
            return response;
        }

        console.log("login successful, token - " + response.value);
        store.commonStore.setToken(response.value);
        return await this.getCurrentUser();
    };

    logout = () => {
        store.commonStore.setToken(null);
        window.localStorage.removeItem("jwt");
        this.user = null;
    };

    updateUserInfo = async (user: UserUpdateModel) => {
        const response = await agent.Profile.updateUser(user);

        if(!response.isSuccessful) {
            return response;
        }

        return await this.getCurrentUser();
    }

    getCurrentUser = async () => {
        try {
            const response = await agent.Auth.current();
            
            if(!response.isSuccessful) {
                return response;
            }

            runInAction(() => this.user = response.value);

            return response;
        } catch (error: any) {
            toast.error(error.response.data.detail);
            throw error;
        }
    };

    register = async (creds: UserRegisterModel) => 
        await agent.Auth.register(creds);
}