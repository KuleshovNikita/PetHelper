import CommonStore from "./api/stores/CommonStore";
import PetStore from "./api/stores/PetStore";
import UserStore from "./api/stores/UserStore";

type Props = {
    commonStore: CommonStore,
    userStore: UserStore,
    petStore: PetStore,
}

export default async function AppLoader({ commonStore, userStore, petStore }: Props) {
    
    let resp = await userStore.getCurrentUser();
    
    if(resp.isSuccessful) {
        await petStore.getUserPets(resp.value.id);
    }

    commonStore.setAppLoaded();
}