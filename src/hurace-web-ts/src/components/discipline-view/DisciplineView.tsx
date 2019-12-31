import React, { useEffect, useState } from 'react';
import styled from 'styled-components';
import { DisciplineData } from '../../interfaces/DisciplineData';
import Axios from 'axios';
import { API_URL } from '../../api';
import { Race } from '../../interfaces/Race';
import { DisciplineViewItem } from './DisciplineViewItem';

const DisciplinePanel = styled.div`
    margin-top: 20px;
    max-height: 500px;
    width: 100%;
    overflow: auto;
`;

export const DisciplineView: React.FC<{ seasonId: number }> = ({
    seasonId
}) => {
    const [disciplineData, setDisciplineData] = useState<
        DisciplineData[] | undefined
    >(undefined);

    useEffect(() => {
        if (disciplineData !== undefined) return;
        async function loadRaces() {
            let response = await Axios.get<Race[]>(
                `${API_URL}/season/${seasonId}/races`
            );
            let disciplineDataset: DisciplineData[] = [];
            response.data.forEach(r => {
                r.raceDate = new Date(r.raceDate);
                let dd = disciplineDataset.find(
                    d => d.discipline.id === r.disciplineId
                );

                if (dd === undefined)
                    disciplineDataset.push({
                        discipline: r.discipline,
                        races: [r]
                    });
                else dd.races.push(r);
            });

            setDisciplineData(disciplineDataset);
        }

        loadRaces();
    }, [disciplineData, seasonId]);

    return (
        <DisciplinePanel>
            {disciplineData?.map(d => (
                <DisciplineViewItem
                    disciplineData={d}
                    key={d.discipline.id}
                ></DisciplineViewItem>
            ))}
        </DisciplinePanel>
    );
};
