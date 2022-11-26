import { createContext, useContext } from "react";
import CommonStore from "./CommonStore";
import PetStore from "./PetStore";
import ScheduleStore from "./ScheduleStore";
import UserStore from "./UserStore";

interface Store {
    commonStore: CommonStore;
    userStore: UserStore;
    petStore: PetStore;
    scheduleStore: ScheduleStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    petStore: new PetStore(),
    scheduleStore: new ScheduleStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
