import { Button, ListItem, ListItemButton, ListItemText, TextField, Typography } from "@mui/material";
import React, { Component, useState } from "react";
import { WalkingSchedule } from "../../models/Pet";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { TimePicker } from "@mui/x-date-pickers";
import ScheduleItem from "./ScheduleItem";

type Props = {
    schedules: WalkingSchedule[]
}

const listItemStyle = {
    bgcolor: "orange",
    borderRadius: "5px",
    mb: 1
}

const listItemButton = {
    bgcolor: "white",
    color: "orange",
    mr: 1,
    "&:hover": {
        bgcolor: "brown"
    }
}

const addScheduleButton = {
    "&:hover": {
        bgcolor: "brown"
    },
    bgcolor: "orange", 
    color: "white"
}

export default function SchedulesList({ schedules }: Props) {
    return(
        <>
            <Typography variant="h4" component="h4">
                Schedules:
            </Typography>
                {
                    schedules && 
                    schedules.length !== 0 &&
                    schedules.map((sch, key) => (
                        <ListItem key={key} component="a" sx={listItemStyle}>
                            <ScheduleItem scheduleItem={sch}/>
                            <Button sx={{...listItemButton, ml: 1}}><EditIcon/></Button>
                            <Button sx={listItemButton}><DeleteIcon/></Button>
                        </ListItem>
                    ))
                }
                
            <Button sx={addScheduleButton}>
                <AddIcon/>
            </Button>
        </>
    );
}