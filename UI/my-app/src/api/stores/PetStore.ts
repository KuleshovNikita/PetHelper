import { makeAutoObservable, runInAction } from "mobx";
import { Pet, PetUpdateModel } from "../../models/Pet";
import { agent } from "../agent";

export default class PetStore {
    pets: Pet[] | null = null;

    constructor() {
        makeAutoObservable(this);
    }

    updatePetInfo = async (pet: PetUpdateModel) => await agent.Pets.updatePet(pet);

    getUserPets = async (id: string) => {
        const result = await agent.Pets.getUserPets(id);

        if(!result.isSuccessful) {
            return result;
        }

        runInAction(() => this.pets = result.value);

        return result;
    }
}