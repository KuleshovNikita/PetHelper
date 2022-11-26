import { ListItemButton, ListItemText, Typography } from "@mui/material";
import { info } from "console";
import React from "react";
import { useNavigate } from "react-router";
import { AnimalType, Pet } from "../../models/Pet";

type Props = {
    pets: Pet[]
}

const listItemStyle = {
    bgcolor: "orange",
    borderRadius: "5px",
    "&:hover": {
        bgcolor: "brown"
    },
    mb: 1
}

export default function PetsList({ pets }: Props) {
    const navigate = useNavigate();

    const getSecondaryPetInfo = (pet: Pet) => {
        let secondaryProps: string[] = [];

        if(pet.animalType !== undefined && AnimalType[pet.animalType]) {
            secondaryProps.push(AnimalType[pet.animalType]);
        }

        if(pet.breed) {
            secondaryProps.push(pet.breed);
        }

        return secondaryProps.join(' | ');
    }

    const openPetPage = (pet: Pet) => {
        navigate(`/pet/${pet.id}`);
    }

    return(
        <>
        <Typography variant="h4" component="h4">
            Pets:
        </Typography>
            {
                pets && 
                pets.length !== 0 &&
                pets.map((pet, key) => (
                    <ListItemButton key={key} component="a" sx={listItemStyle}>
                        <ListItemText primary={pet.name} 
                                      secondary={getSecondaryPetInfo(pet)}
                                      onClick={() => openPetPage(pet)} />
                    </ListItemButton>
                ))
            }
        </>
    );
}