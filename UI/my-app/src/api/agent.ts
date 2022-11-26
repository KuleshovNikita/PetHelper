import axios, { AxiosError, AxiosResponse } from "axios";
import { toast } from "react-toastify";
import { store } from "./stores/Store";
import { redirect } from "react-router";
import { User, UserLoginModel, UserRegisterModel, UserUpdateModel } from "../models/User";
import { EmptyResult, Result } from "../models/Result";
import { Pet, PetUpdateModel, WalkingScheduleUpdateModel } from "../models/Pet";

interface ErrorResponse {
    errors: { detail: string }[];
}

axios.defaults.baseURL = "https://localhost:5001/api";

axios.interceptors.request.use((config) => {
    const token = store.commonStore.token;

    if (token) {
        config.headers!.Authorization = `Bearer ${token}`;
    }

    return config;
});

axios.interceptors.response.use(async (response) => response,
    (error: AxiosError<ErrorResponse>) => {
        const { data, status, headers } = error.response!;
        switch (status) {
            case 400:
                if (data.errors) {
                    console.log(data);
                    const modalStateErrors = [];
                    for (const key in data.errors) {
                        if (data.errors[key]) {
                            console.log('data.errors[key]', data.errors[key])
                            toast.error((data.errors[key] as unknown as Array<string>)[0]);
                            modalStateErrors.push(data.errors[key]);
                        }
                    }
                    throw modalStateErrors.flat();
                } else {
                    toast.error(data.errors);
                }
                break;
            case 401:
                if (headers['www-authenticate']?.startsWith('Bearer error="invalid_token"')) {
                    store.userStore.logout();
                    toast.error('Session has expired - please login again');
                }
                break;
            case 404:
                redirect("/notFound");
                break;
            case 500:
                data.errors.map(e => toast.error(e.detail));                
                break;
        }

        return Promise.reject(error);
    }
);

const responseBody = <T>(response: AxiosResponse<T>) => response.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody),
};

const Auth = {
    current: () => requests.get<Result<User>>("/authentication/current"),
    login: (body: UserLoginModel) => requests.post<Result<string>>("/authentication/login", body),
    logout: () => requests.get<EmptyResult>("/authentication/logout"),
    register: (body: UserRegisterModel) => requests.post<Result<string>>("/authentication/register", body)
}

const Profile = {
    updateUser: (body: UserUpdateModel) => requests.put<EmptyResult>(`/user/${body.id}`, body),
}

const Pets = {
    updatePet: (body: PetUpdateModel) => requests.put<EmptyResult>(`/pet/${body.id}`, body),
    getUserPets: (id: string) => requests.get<Result<Pet[]>>(`/pet/user/${id}`),
}

const Schedules = {
    updateSchedule: (body: WalkingScheduleUpdateModel) => requests.put<EmptyResult>(`/schedule/${body.id}`, body),
}

export const agent = {
    Auth,
    Profile,
    Pets,
    Schedules
};