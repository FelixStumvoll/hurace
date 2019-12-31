import React, { useState, useEffect } from 'react';
import { SeasonViewItem } from './SeasonViewItem';
import styled from 'styled-components';
import Axios from 'axios';
import { API_URL } from '../../api';
import { Season } from '../../interfaces/Season';

const SeasonItemPanel = styled.div`
    display: flex;
    flex-direction: column;
    flex-wrap: wrap;
`;

const SeasonLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 10px;
`;

export const SeasonView: React.FC = () => {
    const [seasons, setSeasons] = useState<Season[] | undefined>(undefined);

    useEffect(() => {
        if (seasons !== undefined) return;
        async function fetchData() {
            var s = await Axios.get<Season[]>(`${API_URL}/season`);

            s.data.map(season => {
                season.endDate = new Date(season.endDate);
                season.startDate = new Date(season.startDate);
                return season;
            });

            setSeasons(s.data);
        }
        fetchData();
    }, [seasons]);

    return (
        <div>
            <SeasonLabel>Alle Saisonen:</SeasonLabel>
            <SeasonItemPanel>
                {seasons?.map(s => (
                    <SeasonViewItem key={s.id} season={s} />
                ))}
            </SeasonItemPanel>
        </div>
    );
};
