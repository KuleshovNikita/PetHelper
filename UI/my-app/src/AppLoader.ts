import CommonStore from "./api/stores/CommonStore";
import PetStore from "./api/stores/PetStore";
import UserStore from "./api/stores/UserStore";
import WalkStore from "./api/stores/WalkStore";

type Props = {
    commonStore: CommonStore,
    userStore: UserStore,
    petStore: PetStore,
    walkStore: WalkStore,
}

export default async function AppLoader({ commonStore, userStore, petStore, walkStore }: Props) {
    let resp = await userStore.getCurrentUser();
    
    if(resp.isSuccessful) {
        await petStore.getUserPets(resp.value.id);
        await walkStore.getPetsWalks(petStore.pets!);
    }

    commonStore.setAppLoaded(true);
}