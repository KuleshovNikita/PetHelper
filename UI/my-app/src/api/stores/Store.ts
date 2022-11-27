import { createContext, useContext } from "react";
import CommonStore from "./CommonStore";
import PetStore from "./PetStore";
import ScheduleStore from "./ScheduleStore";
import UserStore from "./UserStore";
import WalkStore from "./WalkStore";

interface Store {
    commonStore: CommonStore;
    userStore: UserStore;
    petStore: PetStore;
    scheduleStore: ScheduleStore;
    walkStore: WalkStore;
}

export const store: Store = {
    commonStore: new CommonStore(),
    userStore: new UserStore(),
    petStore: new PetStore(),
    scheduleStore: new ScheduleStore(),
    walkStore: new WalkStore(),
}

export const StoreContext = createContext(store);

export function useStore() {
    return useContext(StoreContext);
}
