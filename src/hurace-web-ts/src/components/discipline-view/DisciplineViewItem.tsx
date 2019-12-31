import React from 'react';
import styled from 'styled-components';
import { RaceViewItem } from '../race-view/RaceViewItem';
import { DisciplineData } from '../../interfaces/DisciplineData';

const DisciplinePanel = styled.div`
    margin-bottom: 10px;
    padding: 5px;
`;

const DisciplineLabel = styled.div`
    font-weight: bold;
    font-size: 20px;
    margin-bottom: 5px;
`;

const RacesPanel = styled.div`
    display: flex;
    flex-wrap: wrap;
`;

export const DisciplineViewItem: React.FC<{
    disciplineData: DisciplineData;
}> = ({ disciplineData }) => {
    return (
        <DisciplinePanel>
            <DisciplineLabel>
                {disciplineData.discipline.disciplineName}
            </DisciplineLabel>
            <RacesPanel>
                {disciplineData.races.map(r => (
                    <RaceViewItem key={r.id} race={r} />
                ))}
            </RacesPanel>
        </DisciplinePanel>
    );
};
