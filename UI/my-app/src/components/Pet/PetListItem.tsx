import { Box, Button, TextFieldProps, Typography } from "@mui/material";
import React, { useState } from "react";
import { useStore } from "../../api/stores/Store";
import { AnimalType, Pet } from "../../models/Pet";
import DeleteIcon from '@mui/icons-material/Delete';
import VisibilityIcon from '@mui/icons-material/Visibility';
import { toast } from "react-toastify";
import { listItemButton } from "../../styles/Button/ButtonStyles";
import { useNavigate } from "react-router";

type Props = {
    petItem: Pet,
    removeItem: () => void
}

export default function PetListItem({ petItem, removeItem }: Props) {
    const { petStore } = useStore();
    const navigate = useNavigate();

    const removePet = async () => {
        if(petItem.id !== "") {
            const result = await petStore.removePet(petItem.id)

            if(!result.isSuccessful) {
                toast.error(result.clientErrorMessage);
            } else {
                toast.success("The pet was removed");
                removeItem();
            }
        } else {
            removeItem();
        }
    }

    const getSecondaryPetInfo = () => {
        let secondaryProps: string[] = [];

        if(petItem.animalType !== undefined && AnimalType[petItem.animalType]) {
            secondaryProps.push(AnimalType[petItem.animalType]);
        }

        if(petItem.breed) {
            secondaryProps.push(petItem.breed);
        }

        return secondaryProps.join(' | ');
    }

    const openPetProfile = () => {
        navigate(`/pet/${petItem.id}`);
    }

    return (
        <Box sx={{ display: "grid", gridTemplateColumns: "2fr 1fr" }}>
            <Box sx={{ display: "flex", flexDirection: "column" }}>
                <Typography variant="h4" component="h4" color="white">
                    {petItem.name}
                </Typography>
                <Typography variant="h5" component="h5" color="white">
                    {getSecondaryPetInfo()}
                </Typography>
            </Box>

            <Box sx={{ display: "flex", justifyContent: "end" }}>
                <Button sx={{...listItemButton, ml: 1}} onClick={openPetProfile}>
                    <VisibilityIcon/>
                </Button>
                <Button sx={listItemButton} onClick={removePet}>
                    <DeleteIcon/>
                </Button>
            </Box>
        </Box>
    );
}