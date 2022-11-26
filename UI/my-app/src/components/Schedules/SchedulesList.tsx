import { Button, ListItem, ListItemButton, ListItemText, TextField, Typography } from "@mui/material";
import React, { Component, useState } from "react";
import { WalkingSchedule } from "../../models/Pet";
import EditIcon from '@mui/icons-material/Edit';
import DeleteIcon from '@mui/icons-material/Delete';
import AddIcon from '@mui/icons-material/Add';
import { TimePicker } from "@mui/x-date-pickers";

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
    const [isTextFieldDisabled, setTextFieldDisabled] = useState(true);

    const [start, setStart] = useState<string | null>('');

    let normalizedStart = '';
    let normalizedEnd = '';

    const handleStartChange = (value: string | null, kb: string | undefined) => {
        normalizedStart = value!;
        console.log(value!);
        console.log(kb!);
    }

    const handleEndChange = (value: string | null, kb: string | undefined) => {
        normalizedEnd = value!;
    }

    const normalizeTime = (schedule: WalkingSchedule) => {
        normalizedStart = new Date(schedule.scheduledStart).toLocaleTimeString();
        setStart(normalizedStart);

        normalizedEnd = new Date(schedule.scheduledEnd).toLocaleTimeString();

        return (
            <Typography 
                variant="h4" 
                component="h4"
                color="white">
                    Start: <TimePicker
                                value={start}
                                onChange={(value) => { setStart(value) }}
                                renderInput={(props) => <TextField {...props}/>}/>
            </Typography>
        );
    }

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
                            <ListItemText/>
                            { normalizeTime(sch) }
                            <Button sx={listItemButton}><EditIcon/></Button>
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